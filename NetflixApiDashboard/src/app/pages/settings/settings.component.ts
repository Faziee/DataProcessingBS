import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Clipboard } from '@angular/cdk/clipboard';
import { AuthService, AccountDto } from '../../services/auth.service';
import { HttpErrorResponse } from '@angular/common/http';
import { Router } from '@angular/router';
import { environment } from '../../../../environment';
import { DropdownModule } from 'primeng/dropdown';
import { Password } from 'primeng/password';
import {NgIf} from '@angular/common';

@Component({
  selector: 'app-settings',
  templateUrl: './settings.component.html',
  imports: [
    DropdownModule,
    ReactiveFormsModule,
    Password,
    NgIf
  ],
  styleUrls: ['./settings.component.scss']
})
export class SettingsComponent implements OnInit {
  account: AccountDto | null = null;
  accountForm: FormGroup;
  copied = false;

  paymentMethods = [
    { label: 'Credit Card', value: 'credit_card' },
    { label: 'PayPal', value: 'paypal' },
    { label: 'Bank Transfer', value: 'bank_transfer' }
  ];

  constructor(
    private fb: FormBuilder,
    public authService: AuthService,
    private clipboard: Clipboard,
    private router: Router
  ) {
    this.accountForm = this.fb.group({
      account_Id: [''],
      email: ['', [Validators.email]],
      password: ['', [Validators.minLength(6)]],
      payment_Method: ['', Validators.required],
      trial_End_Date: [''],
    });
  }

  copyApiKey() {
    const apiKey = this.authService.getApiKey() || '';
    this.clipboard.copy(apiKey);
    this.copied = true;
    setTimeout(() => this.copied = false, 2000);
  }

  ngOnInit() {
    this.authService.currentAccount$.subscribe({
      next: (account) => {
        if (account) {
          this.account = account;
          this.accountForm.patchValue({
            email: account.email,
            payment_Method: account.payment_Method,
            trial_End_Date: account.trial_End_Date
          });
          this.accountForm.markAsDirty();
        }
      },
      error: (err) => console.error('Error fetching account', err)
    });
  }

  onSubmit() {
    if (this.accountForm.valid && this.accountForm.dirty) {
      const updates: Partial<AccountDto> = {
        account_Id: this.account?.account_Id,
        email: this.accountForm.get('email')?.value || '',
        password: this.accountForm.get('password')?.value || '',
        payment_Method: this.accountForm.get('payment_Method')?.value || ''
      };

      this.authService.http.put<AccountDto>(
        `${environment.apiUrl}/stored-procedure-update-account-by-id`,
        updates,
        { headers: this.authService.getHeaders() }
      ).subscribe({
        next: (response: AccountDto) => {
          localStorage.setItem('currentAccount', JSON.stringify(response));
          this.authService.currentAccountSubject.next(response);
          alert('Account updated successfully!');
        },
        error: (error: HttpErrorResponse) => {
          alert('Failed to update account: ' + error.message);
        }
      });
    }
  }

  deleteAccount() {
    if (confirm('Are you sure you want to delete your account? This action cannot be undone.')) {
      const accountId = this.account?.account_Id;
      if (accountId) {
        this.authService.http.delete(
          `${environment.apiUrl}/stored-procedure-delete-account/${accountId}`,
          { headers: this.authService.getHeaders() }
        ).subscribe({
          next: () => {
            alert('Account deleted successfully.');
            this.authService.logout(); // Log out the user
            this.router.navigate(['/login']); // Navigate to the login page
          },
          error: (error: HttpErrorResponse) => {
            alert('Failed to delete account: ' + error.message);
          }
        });
      } else {
        alert('Account ID not found. Cannot delete account.');
      }
    }
  }
}
