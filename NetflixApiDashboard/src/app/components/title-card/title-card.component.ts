import { Component, OnInit, ElementRef, ViewChild, Input } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import {ActivatedRoute, RouterLink} from '@angular/router';
import {NgForOf} from '@angular/common';

@Component({
  selector: 'app-title-card',
  templateUrl: './title-card.component.html',
  imports: [
    RouterLink,
    NgForOf
  ],
  styleUrls: ['./title-card.component.scss']
})
export class TitleCardComponent implements OnInit {
 apiData: any[] = [];
  @Input() category: string = 'popular';
  @Input() title: string = 'Popular on Netflix';

  @ViewChild('cardList', { static: false }) cardListRef!: ElementRef;

  constructor(
    private http: HttpClient,
    private route: ActivatedRoute
  ) { }

  ngOnInit() {
    if (!this.category) {
      this.category = 'popular';
    }
    this.fetchMovies();
  }

  fetchMovies() {
    const apiUrl = `http://localhost:5025/movies?category=${this.category}`;
    this.http.get<any>(apiUrl).subscribe(
      (response) => {
        this.apiData = response || [];
        console.log(this.apiData);
      },
      (error) => {
        console.error('Error fetching movie data:', error);
      }
    );
  }
}
