import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router'; // Import Router
import { HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import {DialogService} from 'primeng/dynamicdialog';
import {AddEpisodeComponent} from '../add-episode/add-episode.component';
import {ConfirmationService, MessageService} from 'primeng/api';
import {Button} from 'primeng/button';
import {TrashIcon} from 'primeng/icons';
import {StyleClass} from 'primeng/styleclass';

@Component({
  selector: 'app-series-details',
  templateUrl: './series-details.component.html',
  styleUrls: ['./series-details.component.scss'],
  imports: [CommonModule, Button],

})
export class SeriesDetailsComponent implements OnInit {
  series_Id!: number;
  seriesDetails: any;
  episodes: any[] = [];
  isLoading: boolean = true;

  // Properly inject Router along with ActivatedRoute and HttpClient
  constructor(
    private route: ActivatedRoute,
    private router:Router,
    private http: HttpClient,
    private readonly dialogService:DialogService,
    private readonly messageService: MessageService,
    private confirmService:ConfirmationService,
  ) {}

  ngOnInit() {
    this.series_Id = +this.route.snapshot.paramMap.get('series_Id')!;
    this.fetchSeriesDetails();
    this.fetchEpisodes();
  }

  fetchSeriesDetails() {
    this.http
      .get<any>(`http://localhost:5025/stored-procedure-get-series-by-id/${this.series_Id}`)
      .subscribe(
        (response) => {
          this.seriesDetails = response;
          console.log('Series details response:', response);
        },
        (error) => {
          console.error('Error fetching series details:', error);
        },
        () => {
          this.isLoading = false;
        }

      );
  }

  fetchEpisodes() {
    this.http
      .get<any>(`http://localhost:5025/stored-procedure-get-episodes-by-series-id/${this.series_Id}`)
      .subscribe(
        (response) => {
          console.log('Episodes response:', response); // Debug the API response
          this.episodes = Array.isArray(response) ? response : response.episodes || [];
        },
        (error) => {
          console.error('Error fetching episodes:', error);
        }
      );
  }

  addEpisodeClicked(){

    const dialogRef = this.dialogService.open(AddEpisodeComponent, {
      width: '50%',
      closable:true,
      data:{
        series:this.seriesDetails,
      },
      closeOnEscape: true,
    })
    dialogRef.onClose.subscribe(added => {
      if(added) {
        this.messageService.add({severity: 'success', summary: 'Success', detail: 'Episode added successfully'});
        this.fetchEpisodes();
      }
    });

  }
  deleteSeriesClicked() {
    this.confirmService.confirm({
      message: "Are you sure you want to delete this series?",
    })
    this.http.delete(`http://localhost:5025/stored-procedure-delete-series/${this.series_Id}`).subscribe(() => {
        this.router.navigate(['/series']);
    });

  }

  deleteEpisodeClicked(episode: any) {
    console.log('Episode to delete:', episode);  // Add this to check if the episode object contains episode_Id
    if (!episode || !episode.episode_Id) {
      console.error('Invalid episode data, episode_Id is missing');
      return;
    }
    this.confirmService.confirm({
      message: "Are you sure you want to delete this episode?",
      accept: () => {
        this.http.delete(`http://localhost:5025/stored-procedure-delete-episode/${episode.episode_Id}`).subscribe(() => {
          this.fetchEpisodes();
        });
      }
    });
  }


}
