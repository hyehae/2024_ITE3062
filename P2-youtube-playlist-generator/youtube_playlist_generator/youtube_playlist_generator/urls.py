"""
URL configuration for youtube_playlist_generator project.

The `urlpatterns` list routes URLs to views. For more information please see:
    https://docs.djangoproject.com/en/5.0/topics/http/urls/
Examples:
Function views
    1. Add an import:  from my_app import views
    2. Add a URL to urlpatterns:  path('', views.home, name='home')
Class-based views
    1. Add an import:  from other_app.views import Home
    2. Add a URL to urlpatterns:  path('', Home.as_view(), name='home')
Including another URLconf
    1. Import the include() function: from django.urls import include, path
    2. Add a URL to urlpatterns:  path('blog/', include('blog.urls'))
"""
from django.conf import settings
from django.contrib import admin
from django.urls import include, path
from playlist.views import main_view, get_video_info

urlpatterns = [
    path('admin/', admin.site.urls),
    path('', main_view, name='main_view'),
    path('accounts/', include('allauth.urls')),
    path('get_video_info/', get_video_info,name='get_video_info'),
]

if not settings.DEBUG:
    from django.views.static import serve
    urlpatterns += [
        path('static/<path:path>', serve, {'document_root': settings.STATIC_ROOT}),
    ]
