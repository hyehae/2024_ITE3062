import os
import shutil
from django.core.management.base import BaseCommand

class Command(BaseCommand):
    help = 'Copy contents from credentials.json to client_secret.json'

    def handle(self, *args, **kwargs):
        base_dir = os.path.dirname(os.path.dirname(os.path.dirname(os.path.dirname(os.path.abspath(__file__)))))

        # Debug output
        self.stdout.write(base_dir)
        
        credentials_file = os.path.join(base_dir, 'credentials2.json')
        client_secret_file = os.path.join(base_dir, 'client_secrets.json')

        # Debug output
        self.stdout.write(credentials_file)

        if os.path.exists(credentials_file):
            shutil.copy(credentials_file, client_secret_file)
            self.stdout.write(self.style.SUCCESS('Successfully copied credentials.json to client_secret.json'))
        else:
            self.stdout.write(self.style.ERROR('credentials.json not found'))
