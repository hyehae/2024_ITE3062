import os
from google.oauth2.credentials import Credentials
from google_auth_oauthlib.flow import InstalledAppFlow
from googleapiclient.discovery import build
from oauth2client import client
from oauth2client import tools
from oauth2client.file import Storage
import googleapiclient.errors

# OAuth 2.0 클라이언트 ID 파일 경로
CLIENT_SECRETS_FILE = "client_secrets.json"

#CLIENT_SECRETS_FILE = "credentials.json"
#CLIENT_TOKEN_FILE = "client_secrets.json"

# API 서비스 이름 및 버전
API_SERVICE_NAME = 'youtube'
API_VERSION = 'v3'

# 권한 범위
SCOPES = ['https://www.googleapis.com/auth/youtube.force-ssl']

def get_authenticated_service():
    try:
        credentials = Credentials.from_authorized_user_file(CLIENT_SECRETS_FILE)
    except ValueError as e: # first run with new secret.json (no refresh_token yet)
        flow = InstalledAppFlow.from_client_secrets_file(CLIENT_SECRETS_FILE, SCOPES)
        credentials = flow.run_local_server()
        with open(CLIENT_SECRETS_FILE, 'w') as file:
            file.write(credentials.to_json())
    return build(API_SERVICE_NAME, API_VERSION, credentials=credentials)

if __name__ == "__main__":
    service = get_authenticated_service()

