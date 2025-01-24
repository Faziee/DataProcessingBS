import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { AuthService, AccountDto } from '../../services/auth.service';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent implements OnInit {
  userEmail: string | null = null;

  constructor(private readonly authService: AuthService) {}

  ngOnInit(): void {
    this.authService.currentAccount$.subscribe((account: AccountDto | null) => {
      this.userEmail = account?.email || null;
    });
  }

  onSignOutClicked(): void {
    this.authService.logout();
  }
}
