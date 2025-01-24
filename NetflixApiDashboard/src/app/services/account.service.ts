import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AuthService, AccountDto } from './auth.service';

@Injectable({
  providedIn: 'root',
})
export class AccountService {
  constructor(private authService: AuthService) {}

  getLoggedInAccountDetails(): Observable<AccountDto> {
    const currentAccount = this.authService.currentAccountSubject.value;
    if (!currentAccount || !currentAccount.account_Id) {
      throw new Error('No account is currently logged in.');
    }

    // Reuse AuthService's method to get the account by ID
    return this.authService.getAccountById(currentAccount.account_Id);
  }
}
