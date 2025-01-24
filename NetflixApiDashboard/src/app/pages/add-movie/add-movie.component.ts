import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { DropdownModule } from 'primeng/dropdown';
import { InputText } from 'primeng/inputtext';
import { StyleClass } from 'primeng/styleclass';
import { lastValueFrom } from 'rxjs';
import {Checkbox} from 'primeng/checkbox';

export interface Movie {
  genre_Id: number;
  title: string;
  age_Rating: string;
  quality: string;
  has_Subtitles: boolean;
}

@Component({
  selector: 'app-add-movie',
  templateUrl: './add-movie.component.html',
  imports: [
    FormsModule,
    ReactiveFormsModule,
    DropdownModule,
    InputText,
    StyleClass,
    Checkbox,
  ],
})
export class AddMovieComponent implements OnInit {
  movieForm!: FormGroup;

  genres = [
    { label: 'Action', value: 1 },
    { label: 'Comedy', value: 2 },
    { label: 'Drama', value: 3 },
  ];

  ageRatings = [
    { label: 'G', value: 'G' },
    { label: 'PG', value: 'PG' },
    { label: 'PG-13', value: 'PG-13' },
    { label: 'R', value: 'R' },
  ];

  qualities = [
    { label: 'String', value: 'string' },
    { label: 'SD', value: 'SD' },
    { label: 'HD', value: 'HD' },
    { label: '4K', value: '4K' },
  ];

  constructor(
    private readonly fb: FormBuilder,
    private readonly http: HttpClient,
    private readonly dialogRef: DynamicDialogRef,
    private readonly dynamicDialogConfig: DynamicDialogConfig
  ) {}

  ngOnInit(): void {
    this.movieForm = this.fb.group({
      title: ['', Validators.required],
      genre_Id: [null, Validators.required],
      age_Rating: ['', Validators.required],
      quality: ['', Validators.required],
      has_Subtitles: [false],
    });
  }

  async onSubmit(): Promise<void> {
    if (this.movieForm.valid) {
      try {
        await lastValueFrom(
          this.http.post('http://localhost:5025/stored-procedure-create-movie', this.movieForm.value)
        );
        this.dialogRef.close(true);
      } catch (error) {
        console.error('Error adding movie:', error);
      }
    }
  }

  onCancel(): void {
    this.dialogRef.close(false);
  }
}
