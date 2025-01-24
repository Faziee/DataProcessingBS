import {Component, OnInit} from '@angular/core';
import {ProgressSpinner} from 'primeng/progressspinner';
import {Card} from 'primeng/card';
import { Router } from '@angular/router';
import {AddSeriesModalComponent} from '../add-series-modal/add-series-modal.component';
import {DialogService} from 'primeng/dynamicdialog';
import {AuthService} from '../../services/auth.service';
import {NgForOf, NgIf} from '@angular/common';
import {AddMovieComponent} from '../add-movie/add-movie.component';

@Component({
  selector: 'app-movie',
  imports: [
    ProgressSpinner,
    Card,
    NgForOf,
    NgIf
  ],
  templateUrl: './movie.component.html',
  styleUrl: './movie.component.scss'
})
export class MovieComponent implements OnInit{
  isLoading: boolean = true;
  moviesData: any[] = [];

  constructor(private authService: AuthService,
  private readonly router: Router,
  private readonly dialogService:DialogService
  ) {}

  ngOnInit() {
    this.getMovies();
  }

  getMovies(): void {
    this.authService.http.get('http://localhost:5025/stored-procedure-get-movies').subscribe(
      (response: any) => {
        this.moviesData = response;
        this.isLoading = false;
      },
      (error: any) => {
        console.error('Error fetching series data:', error);
        this.isLoading = false;
      }
    );
  }

  onCardClick(series: any) {
    this.router.navigate(['/details-movie', series.series_Id]);
  }

  addMovieClicked() {
    this.dialogService.open(AddMovieComponent, {
      width: '50%',
      closable:true,
    }).onClose.subscribe(added => {
      if(added) {
        this.getMovies();
      }
    })
  }
}
