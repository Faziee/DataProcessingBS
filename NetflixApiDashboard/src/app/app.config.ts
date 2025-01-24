import { ApplicationConfig } from '@angular/core';
import { provideRouter } from '@angular/router';
import { routes } from './app.routes';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { providePrimeNG } from 'primeng/config';
import Aura from '@primeng/themes/aura';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import {MessageService} from 'primeng/api';

export const appConfig: ApplicationConfig = {
  providers: [
    MessageService,
    provideRouter(routes),
    provideAnimationsAsync(),
    providePrimeNG({
      theme: {
        preset: Aura
      }
    }),
    provideHttpClient(
      withInterceptors([
        (req, next) => {
          const apiKey = localStorage.getItem('apiKey');
          if (apiKey) {
            req = req.clone({
              setHeaders: {
                'api-key': apiKey
              }
            });
          }
          return next(req);
        }
      ])
    ),
  ]
};
