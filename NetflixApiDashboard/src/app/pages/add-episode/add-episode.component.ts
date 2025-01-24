import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { FormBuilder, FormGroup, Validators, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { Button } from 'primeng/button';
import { DropdownModule } from 'primeng/dropdown';
import { InputNumber } from 'primeng/inputnumber';
import { InputText } from 'primeng/inputtext';
import { Card } from 'primeng/card';
import { lastValueFrom } from 'rxjs';
import {StyleClass} from 'primeng/styleclass';

export interface Episode {
  Media_Id: number;
  Series_Id: number;
  Genre_Id: number;
  Series_Title: string;
  Season_Number: number;
  Episode_Number: number;
  Title: string;
  Duration: number;
  Age_Rating: string;
  Quality: string;
}

@Component({
  selector: 'app-add-episode',
  templateUrl: './add-episode.component.html',
  imports: [FormsModule, ReactiveFormsModule, Button, DropdownModule, InputNumber, InputText, Card, StyleClass]
})
export class AddEpisodeComponent implements OnInit{
  episodeForm!: FormGroup;
  series!:any;
  seriesTitle = '';

  ageRatings = [
    { label: 'G', value: 'G' },
    { label: 'PG', value: 'PG' },
    { label: 'PG-13', value: 'PG-13' },
    { label: 'R', value: 'R' }
  ];

  qualityOptions = [
    { label: 'HD', value: 'HD' },
    { label: '4K', value: '4K' },
    { label: 'SD', value: 'SD' }
  ];

  constructor(
    private http: HttpClient,
    private fb: FormBuilder,
    private readonly dynamicDialogConfig: DynamicDialogConfig,
    private readonly dialogRef: DynamicDialogRef,
  ) {
    this.series = this.dynamicDialogConfig.data.series;

  }

ngOnInit() {
  this.episodeForm = this.fb.group({
    series_id: [this.series.series_Id],
    Media_Id: [0],
    Genre_Id: [3],
    Series_Title: [this.series.title, Validators.required],
    Season_Number: [1, [Validators.required, Validators.min(1)]],
    Episode_Number: [1, [Validators.required, Validators.min(1)]],
    Title: ['', Validators.required],
    Duration: [30, [Validators.required, Validators.min(1)]],
    Age_Rating: ['', Validators.required],
    Quality: ['', Validators.required]
  });
}

  async onSubmit(): Promise<void> {
    if (this.episodeForm.valid) {
      try {
        await lastValueFrom(
          this.http.post('http://localhost:5025/stored-procedure-create-episode', this.episodeForm.value)
        );
        this.dialogRef.close(true);
      } catch (error) {
        console.error('Error adding episode:', error);
      }
    }
  }

  onCancel(): void {
    this.dialogRef.close(false);
  }
}
