from django.shortcuts import render
from django.http import JsonResponse
from googleapiclient.discovery import build
from google.oauth2.credentials import Credentials
from django.views.decorators.csrf import csrf_exempt
from dotenv import load_dotenv
import os
import requests
import re
import json

# .env 파일 활성화
load_dotenv()
api_key = os.getenv('YOUTUBE_API_KEY')

# 유튜브 API 클라이언트 빌드
youtube = build('youtube', 'v3', developerKey=api_key)

def my_view(request):
    return render(request, 'video_info_form.html')

@csrf_exempt
def get_video_info(request):
    if request.method == 'POST':
        # 요청 바디에서 JSON 데이터 파싱
        data = json.loads(request.body.decode('utf-8'))
        youtube_link = data.get('youtube_link')
        if youtube_link:
            # 유튜브 API 엔드포인트 URL
            video_id_index = youtube_link.find("v=")
            video_id = youtube_link[video_id_index + 2:]
            #video_id = youtube_link.split('/')[-1].split('?')[0]  # 링크에서 동영상 ID 추출
            #api_key = "abcd"  # 본래는 안전한 방법으로 API 키를 관리해야 합니다
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

                    return JsonResponse({
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

def extract_music_info(comments):
    music_info = []
    pattern = r'(?P<time>\d{2}:\d{2})\s+(?P<title>.+?)\s*-\s*(?P<artist>.+)'
    #pattern = r'(?P<time>\d{2}:\d{2})\s+(?P<title>.+?)\s+-\s+(?P<artist>.+)'

    for comment in comments:
        match = re.search(pattern, comment)
        if match:
            time = match.group('time')
            title = match.group('title')
            artist = match.group('artist')
            music_info.append({'time': time, 'title': title, 'artist': artist})
    
    return music_info

# 테스트를 위한 댓글 데이터
# comments = [
#     "00:00    Newness - Musiq Soulchild",
#     "03:39    Deadly - Beharie",
#     "05:28    I Like (Feat. Mary J. Blige) - Diddy & Mary J. Blige",
#     "07:11    New Day - Crush",
#     "08:10    No One - brb. & HYBS",
#     "10:21    Blinding Lights (BRLLNT Remix) - The Weeknd",
#     "11:32    In The Air - Crush",
#     "13:00    Miyazaki - Gallant"
# ]

# 댓글 분석하여 음악 정보 추출
# music_info = extract_music_info(comments)

# 추출된 음악 정보 출력
    # for info in music_info:
    #     print("Time:", info['time'])
    #     print("Title:", info['title'])
    #     print("Artist:", info['artist'])
    #     print()


# def create_playlist(title, description):
#     # 새로운 재생목록 생성 요청
#     request = youtube.playlists().insert(
#         part="snippet",
#         body={
#           "snippet": {
#             "title": title,
#             "description": description
#           }
#         }
#     )
    
#     # API를 호출하여 새로운 재생목록 생성
#     response = request.execute()
#     return response.get('id')

# def add_video_to_playlist(playlist_id, video_id):
#     # 재생목록에 비디오 추가 요청
#     request = youtube.playlistItems().insert(
#         part="snippet",
#         body={
#           "snippet": {
#             "playlistId": playlist_id,
#             "position": 0,
#             "resourceId": {
#               "kind": "youtube#video",
#               "videoId": video_id
#             }
#           }
#         }
#     )

#     # API를 호출하여 비디오를 재생목록에 추가
#     response = request.execute()
#     return response.get('id')

# # 새로운 재생목록 생성
# playlist_id = create_playlist("My New Playlist", "This is a test playlist.")

# # 음악을 추가할 경우
# # 음악의 동영상 ID와 새로 생성한 재생목록 ID를 사용하여 음악을 재생목록에 추가
# video_id = 'YOUR_VIDEO_ID'  # 음악의 유튜브 동영상 ID
# add_video_to_playlist(playlist_id, video_id)