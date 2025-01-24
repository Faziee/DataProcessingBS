import { Component } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';
import {TitleCardComponent} from '../../components/title-card/title-card.component';
import {FooterComponent} from '../../layout/footer/footer.component';
import {NavbarComponent} from '../../layout/navbar/navbar.component';

@Component({
  selector: 'app-home',
  templateUrl: './home-page.component.html',
  imports: [
    TitleCardComponent,
  ],
  styleUrls: ['./home-page.component.scss']
})
export class HomePageComponent {
  heroBanner = '../../assets/hero_banner.jpg';
  heroTitle = '../../assets/hero_title.png';
  playIcon = '../../assets/play_icon.png';
  infoIcon = '../../assets/info_icon.png';

  constructor(
    private readonly authService: AuthService,
    private readonly router: Router,
  ) {}

  onLogoutClicked() {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}
