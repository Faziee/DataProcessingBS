import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import {HttpClient} from '@angular/common/http';
import {environment} from '../../../../environment';
import {lastValueFrom} from 'rxjs'; //

@Component({
  selector: 'app-api',
  templateUrl: './api.component.html',
  imports: [
    FormsModule,
    CommonModule,
  ],
  styleUrls: ['./api.component.scss']
})
export class APIComponent implements OnInit {
  randomNumber: string = '';
  media: any = null;
  isLoading: boolean = false;
  invalidInput: boolean = false;
  genres: any[] = [];
  movies: any[] = []; // Add this property to hold movies
  active: any;

  constructor(
    private authService: AuthService,
    private http:HttpClient,
  ) {}

  searchMedia(): void {
    if (isNaN(Number(this.randomNumber)) || this.randomNumber.trim() === '') {
      this.invalidInput = true;
      return;
    }

    this.invalidInput = false;
    this.isLoading = true;
    this.authService.getMediaById(this.randomNumber).subscribe(
      (response: any) => {
        this.media = response;
        this.isLoading = false;
      },
      (error: any) => {
        console.error('Error fetching media data:', error);
        this.isLoading = false;
      }
    );
  }

  fetchGenres(): void {
    this.authService.getGenres().subscribe(
      (response: any) => {
        this.genres = response;
        console.log(this.genres);
      },
      (error: any) => {
        console.error('Error fetching genres:', error);
      }
    );
  }

  ngOnInit(): void {
    this.fetchGenres(); // Fetch genres when the component initializes
  }

  onGenreButtonClicked(type: string) {
    this.active = type;
   this.fetchMoviesByGenreType(type);
  }

  private fetchMoviesByGenreType(type: string) {
      this.authService.getMoviesByGenre(type).subscribe((movies:any) => {
        this.movies = movies;
      })

  }
}
