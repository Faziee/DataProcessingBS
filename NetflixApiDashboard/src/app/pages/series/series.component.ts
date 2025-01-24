import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import {DialogService} from 'primeng/dynamicdialog';
import {AddEpisodeComponent} from '../add-episode/add-episode.component';
import {ProgressSpinner} from 'primeng/progressspinner';
import {Card} from 'primeng/card';
import {AddSeriesModalComponent} from '../add-series-modal/add-series-modal.component'; // Make sure to import Router

@Component({
  selector: 'app-series',
  templateUrl: './series.component.html',
  styleUrls: ['./series.component.scss'],
  imports: [CommonModule, ProgressSpinner, Card],
})
export class SeriesComponent implements OnInit {
  seriesData: any[] = [];
  isLoading: boolean = true;

  constructor(private authService: AuthService, private readonly dialogService:DialogService,
              private readonly router: Router
  ) {}

  ngOnInit(): void {
    this.getSeries();
  }

  getSeries(): void {
    this.authService.http.get('http://localhost:5025/stored-procedure-get-series').subscribe(
      (response: any) => {
        this.seriesData = response;  // Store the series data
        this.isLoading = false;  // Set loading to false after data is fetched
      },
      (error: any) => {
        console.error('Error fetching series data:', error);
        this.isLoading = false;  // Set loading to false if there's an error
      }
    );
  }


  onCardClick(series: any) {
    this.router.navigate(['/series-details', series.series_Id]);

  }

  addSeriesClicked() {
    this.dialogService.open(AddSeriesModalComponent, {
      width: '50%',
      closable:true,
    }).onClose.subscribe(added => {
      if(added) {
        this.getSeries();
      }
    })
  }
}
