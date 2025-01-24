import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { AuthService, SignupRequest } from '../../services/auth.service';
import { CommonModule } from '@angular/common';
import {Button} from 'primeng/button';
import {Message} from 'primeng/message';
import {DropdownModule} from 'primeng/dropdown';
import {Password} from 'primeng/password';
import {Checkbox} from 'primeng/checkbox';
import {InputText} from 'primeng/inputtext';
import {DialogModule} from 'primeng/dialog';
import {DialogService} from 'primeng/dynamicdialog';
import {GifDisplayComponent} from '../../../assets/EasterEggGif.component';


type SignState = 'Sign In' | 'Sign Up';

@Component({
  selector: 'app-login',
  templateUrl: './login-page.component.html',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    CommonModule,
    Button,
    Message,
    DropdownModule,
    Password,
    Checkbox,
    InputText,
  ]
})
export class LoginComponent implements OnInit {
  loginForm!: FormGroup;
  errorMessage: string = '';
  isLoading: boolean = false;
  returnUrl: string = '/home';
  signState: SignState = 'Sign In';

  readonly paymentMethods = [
    {value: 'ING', label: 'ING'},
    {value: 'RABOBANK', label: 'RABOBANK'},
    {value: 'coins', label: 'Coins'}
  ];

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router,
    private route: ActivatedRoute,
    private dialogService: DialogService,
  ) {
    this.initForm();
  }

  private initForm(): void {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(4)]],
      apiKey: ['', [Validators.required, Validators.minLength(4)]],
      paymentMethod: ['ING']
    });
  }

  ngOnInit(): void {
    this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/home';
    if (this.authService.isLoggedIn()) {
      this.router.navigate([this.returnUrl]);
    }
  }

  toggleSignState(): void {
    this.signState = this.signState === 'Sign In' ? 'Sign Up' : 'Sign In';
    this.errorMessage = '';
    this.updateFormValidation();
  }

  private updateFormValidation(): void {
    const apiKeyControl = this.loginForm.get('apiKey');

    if (this.signState === 'Sign Up') {
      apiKeyControl?.clearValidators();
      this.loginForm.patchValue({apiKey: ''});
    } else {
      apiKeyControl?.setValidators([Validators.required, Validators.minLength(4)]);
    }

    apiKeyControl?.updateValueAndValidity();
  }

  onSubmit(): void {
    if (!this.loginForm.valid) {
      this.loginForm.markAllAsTouched();
      return;
    }

    this.isLoading = true;
    this.errorMessage = '';

    if (this.signState === 'Sign In') {
      this.handleSignIn();
    } else {
      this.handleSignUp();
    }
  }

  private handleSignIn(): void {
    const {email, password, apiKey} = this.loginForm.value;

    this.authService.setApiKeyForService(apiKey);
    this.authService.login(email, password).subscribe({
      next: () => {
        this.router.navigate([this.returnUrl]);
      },
      error: (error) => {
        this.isLoading = false;
        this.errorMessage = error.message || 'An error occurred during login';
      },
      complete: () => this.isLoading = false
    });
  }

  private handleSignUp(): void {
    const {email, password, paymentMethod} = this.loginForm.value;

    const signupData: SignupRequest = {
      email,
      password,
      payment_Method: paymentMethod,
      blocked: false,
      is_Invited: true,
      trial_End_Date: new Date().toISOString().split('T')[0]
    };

    this.authService.signup(signupData).subscribe({
      next: () => {
        this.router.navigate(['/home']);
      },
      error: (error) => {
        this.isLoading = false;
        this.errorMessage = error.message || 'An error occurred during sign up';
      },
      complete: () => this.isLoading = false
    });
  }

  // Form getters for template
  get emailControl() {
    return this.loginForm.get('email');
  }

  get passwordControl() {
    return this.loginForm.get('password');
  }

  get apiKeyControl() {
    return this.loginForm.get('apiKey');
  }

  get paymentMethodControl() {
    return this.loginForm.get('paymentMethod');
  }

  // Template helper methods
  get isSignIn(): boolean {
    return this.signState === 'Sign In';
  }

  get isSignUp(): boolean {
    return this.signState === 'Sign Up';
  }

  get submitButtonText(): string {
    return this.isLoading ? 'Processing...' : this.signState;
  }

  needHelpClicked() {
    const ref = this.dialogService.open(GifDisplayComponent, {
      data: {
        title: 'Need Help?',
        gifUrl: '/assets/no-idea.gif',
        description: 'Did you try to turn it on and off again? '
      }
    });
  }
}

