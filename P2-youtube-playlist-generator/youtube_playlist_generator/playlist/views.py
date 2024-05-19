from django.shortcuts import render
from django.http import JsonResponse
from googleapiclient.discovery import build
from google.oauth2.credentials import Credentials
from django.views.decorators.csrf import csrf_exempt
from youtube_playlist_generator.oauth2 import get_authenticated_service
from dotenv import load_dotenv
import os
import requests
import re
import json

# .env 파일 활성화
load_dotenv()
api_key = os.getenv('YOUTUBE_API_KEY')

def extract_music_info(comments):
    music_info = []
    patterns = [
        r'(?P<time>\d{2}:\d{2})\s+(?P<title>.+?)\s*-\s*(?P<artist>.+)',
        r'(?P<time>\d{2}:\d{2})\s+(?P<title>.+)'
    ]
    #pattern = r'(?P<time>\d{2}:\d{2})\s+(?P<title>.+?)\s+-\s+(?P<artist>.+)'

    for comment in comments:
        for pattern in patterns:
            match = re.search(pattern, comment)
            if match:
                #time = match.group('time')
                title = match.group('title')
                artist = match.groupdict().get('artist', None)
                music_info.append({'title': title, 'artist': artist})
                break
    
    return music_info

def create_playlist_and_add_music(youtube, music_info):
    # 새로운 재생목록 생성 요청
    playlist_request = youtube.playlists().insert(
        part="snippet, status",
        body={
          "snippet": {
            "title": "New Playlist",
            "description": "Created by youtube playlist generator"
          },
          "status": {
              "privacyStatus": 'private' # 재생목록 공개 설정
          }
        }
    )
    
    # API를 호출하여 새로운 재생목록 생성
    playlist_response = playlist_request.execute()
    playlist_id = playlist_response['id']

    # music_info(list) 형식: {'title': title, 'artist': artist}
    # 음악 추가
    print("video request")
    for music in music_info:
        # 음악 정보 추출
        artist = music['artist']
        title = music['title']

        # 음악 검색
        video_request = youtube.search().list (
            q=f'{artist} {title}',
            part='snippet',
            type='video',
            maxResults=1  # 검색 결과 중 첫 번째 동영상 사용
        )
        video_response = video_request.execute()
        video_id = video_response['items'][0]['id']['videoId']

        # 검색된 음악을 재생목록에 추가
        print("add video request")
        youtube.playlistItems().insert(
            part="snippet",
            body={
                "snippet": {
                    "playlistId": playlist_id,
                    "resourceId": {
                        "kind": "youtube#video",
                        "videoId": video_id
                    }
                }
            }
        ).execute()

    return playlist_id

def main_view(request):
    return render(request, 'video_info_form.html')

@csrf_exempt
def get_video_info(request):
    if request.method == 'POST':
        # 요청 바디에서 JSON 데이터 파싱
        data = json.loads(request.body.decode('utf-8'))
        youtube_link = data.get('youtube_link')
        if youtube_link:
            # 유튜브 API 엔드포인트 URL (링크에서 동영상 ID 추출)
            video_id_index = youtube_link.find("v=")
            video_id = youtube_link[video_id_index + 2:]

            youtube = get_authenticated_service()

            url = f"https://www.googleapis.com/youtube/v3/videos?id={video_id}&key={api_key}&part=snippet"

            try:
                # API를 호출하여 동영상 정보 요청
                response = requests.get(url)
                data = response.json()

                # 응답에서 동영상 정보 추출
                if 'items' in data and len(data['items']) > 0:
                    video_info = data['items'][0]['snippet']
                    description = video_info.get('description', '') # 동영상 설명
                    # 동영상 설명을 바탕으로 comment 생성
                    comments = description.split('\n')
                    # comments에서 음악 정보 추출
                    music_info = extract_music_info(comments)
                    NewPlaylistId = create_playlist_and_add_music(youtube, music_info)

                    return JsonResponse({
                        'new_playlist': NewPlaylistId,
                        'music_info': music_info,
                        'title': video_info['title'],  # 동영상 제목
                        'description': video_info.get('description', ''),  # 동영상 설명
                        'channel_title': video_info['channelTitle'],  # 채널 제목
                    })
                    # return render(request, 'print_info.html', {
                    #     'music_info': music_info,
                    #     'title': video_info['title'],                
                    #     'description': video_info.get('description', ''),               
                    #     'channel_title': video_info['channelTitle']
                    # })
                else:
                    return JsonResponse({'error': 'Failed to fetch video info.'}, status=400)
            
            except Exception as e:
                return JsonResponse({'error': str(e)}, status=500)
        
    return JsonResponse({'error': 'Invalid request.'}, status=400)
