import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { BehaviorSubject, Observable, map, catchError, throwError } from 'rxjs';
import { environment } from '../../../environment';
import {DialogService} from 'primeng/dynamicdialog';
import {Router} from '@angular/router';

export interface SignupRequest {
  email: string;
  password: string;
  payment_Method: string;
  blocked?: boolean;
  is_Invited?: boolean;
  trial_End_Date?: string;
}

export interface Account {
  account_Id?: number;
  email: string;
  password: string;
  payment_Method: string;
  blocked?: boolean;
  is_Invited?: boolean;
  trial_End_Date?: string;
}

export interface AccountDto {
  account_Id: number;
  email: string;
  password: string;
  payment_Method: string;
  blocked: boolean;
  is_Invited: boolean;
  trial_End_Date: string | null;
}

export interface LoginResponse {
  account: AccountDto;
  apiKey: string;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  public currentAccountSubject = new BehaviorSubject<AccountDto | null>(null);
  private apiKey: string | null = null;
  public currentAccount$ = this.currentAccountSubject.asObservable();

  constructor(
    public http: HttpClient,
    private router:Router,
  ) {
    this.loadStoredAuth();
  }

  private loadStoredAuth(): void {
    const savedAccount = localStorage.getItem('currentAccount');
    this.apiKey = localStorage.getItem('apiKey');
    if (savedAccount) {
      this.currentAccountSubject.next(JSON.parse(savedAccount));
    }
  }
  public getHeaders(): HttpHeaders {
    let headers = new HttpHeaders({
      'Content-Type': 'application/json'
    });

    if (this.apiKey) {
      headers = headers.set('api-key', this.apiKey);
    }

    return headers;
  }

  setApiKeyForService(apiKey: string): void {
    this.setApiKey(apiKey);
  }

  //signup method
  signup(signupData: SignupRequest): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(
      `${environment.apiUrl}/stored-procedure-create-account`,
      {
        email: signupData.email,
        password: signupData.password,
        payment_Method: signupData.payment_Method,
        blocked: signupData.blocked || false,
        is_Invited: signupData.is_Invited || false,
        trial_End_Date: signupData.trial_End_Date || null
      },
      { headers: this.getHeaders() }
    ).pipe(
      map(response => {
        this.setCurrentAccount(response.account);
        if (response.apiKey) {
          this.setApiKey(response.apiKey);
          alert("Dont forget to copy this api key you will only see it once: " +  this.apiKey);
        }
        return response;
      }),
      catchError(error => {
        if (error.status === 400) {
          return throwError(() => new Error('Account creation failed. Please check your information.'));
        }
        return throwError(() => error);
      })
    );
  }

  login(email: string, password: string): Observable<LoginResponse> {
    return this.http.get<AccountDto>(
      `${environment.apiUrl}/stored-procedure-get-account-by-email/${email}`,
      { headers: this.getHeaders() }
    ).pipe(
      map(accountDto => {
        this.setCurrentAccount(accountDto);
        return {
          account: accountDto,
          apiKey: this.apiKey || ''
        };
      }),
      catchError(error => {
        if (error.status === 404) {
          return throwError(() => new Error('Account not found'));
        }
        return throwError(() => error);
      })
    );
  }

  createAccount(accountData: Omit<Account, 'account_id'>): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(
      `${environment.apiUrl}/stored-procedure-create-account`,
      accountData,
      { headers: this.getHeaders() }
    ).pipe(
      map(response => {
        this.setCurrentAccount(response.account);
        return response;
      })
    );
  }

  getAccountById(accountId: number): Observable<AccountDto> {
    return this.http.get<AccountDto>(
      `${environment.apiUrl}/stored-procedure-get-account-by-id/${accountId}`,
      { headers: this.getHeaders() }
    );
  }

  logout(): void {
    localStorage.removeItem('currentAccount');
    localStorage.removeItem('apiKey');
    this.apiKey = null;
    this.currentAccountSubject.next(null);
    this.router.navigate(['/login']);
  }

  private setCurrentAccount(account: AccountDto): void {
    localStorage.setItem('currentAccount', JSON.stringify(account));
    this.currentAccountSubject.next(account);
  }

  private setApiKey(apiKey: string): void {
    localStorage.setItem('apiKey', apiKey);
    this.apiKey = apiKey;
  }

  getApiKey(): string | null {
    return this.apiKey;
  }

  isLoggedIn(): boolean {
    return this.currentAccountSubject.value !== null && this.apiKey !== null;
  }

  getMediaById(id: string): Observable<any> {
    return this.http.get(`${environment.apiUrl}/stored-procedure-get-media-by-id/${id}`);
  }

  getGenres(): Observable<any> {
    return this.http.get(`${environment.apiUrl}/stored-procedure-get-genres`);
  }

  getMoviesByGenre(genreType: string): Observable<any> {
    return this.http.get(`${environment.apiUrl}/stored-procedure-get-movies-by-genre/${genreType}`);
  }

}
