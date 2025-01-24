import { Routes } from '@angular/router';
import { HomePageComponent } from './pages/home-page/home-page.component';
import {authGuard} from './guards/auth.guard';
import {LoginComponent} from './pages/login-page/login-page.component';
import {FooterComponent}  from './layout/footer/footer.component';
import {SettingsComponent} from './pages/settings/settings.component';
import {APIComponent} from './pages/api/api.component';
import {SeriesComponent} from './pages/series/series.component';
import {SeriesDetailsComponent} from './pages/series-details/series-details.component';
import {AddEpisodeComponent} from './pages/add-episode/add-episode.component';
import {MovieComponent} from './pages/movie/movie.component';

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'home',
    pathMatch: 'full'
  },
  {
    path: 'login',
    component: LoginComponent
  },
  {
    path: 'home',
    component: HomePageComponent,
    canActivate: [authGuard]
  },
  {
    path: 'footer',
    component: FooterComponent,
    canActivate: [authGuard]
  },
  {
    path: 'settings',
    component: SettingsComponent,
    canActivate: [authGuard]
  },
  {
    path: 'api',
    component: APIComponent,
    canActivate: [authGuard]
  },
  {
    path: 'series',
    component: SeriesComponent,
    canActivate: [authGuard]
  },
  {
    path: 'series-details/:series_Id',
    component: SeriesDetailsComponent,
    canActivate: [authGuard]
  },
  {
    path: 'add-episode',
    component: AddEpisodeComponent,
    canActivate: [authGuard]
  },
  {
    path: 'movie',
    component: MovieComponent,
    canActivate: [authGuard]
  },
  {
    path: '**',
    redirectTo: 'home'
  }
]
