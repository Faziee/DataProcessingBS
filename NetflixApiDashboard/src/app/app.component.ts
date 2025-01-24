import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import {NavbarComponent} from './layout/navbar/navbar.component';
import {AuthService} from './services/auth.service';
import {Dialog} from 'primeng/dialog';
import {DialogService} from 'primeng/dynamicdialog';
import {FooterComponent} from './layout/footer/footer.component';
import {Message} from 'primeng/message';
import {ConfirmationService, MessageService} from 'primeng/api';
import {ConfirmDialog} from 'primeng/confirmdialog';
import {Toast} from 'primeng/toast';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, Dialog, NavbarComponent, FooterComponent, ConfirmDialog, Toast],
  providers: [DialogService, ConfirmationService],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'NetflixApiDashboard';

  constructor(
    public readonly authService:AuthService,
  ) {
  }
}
