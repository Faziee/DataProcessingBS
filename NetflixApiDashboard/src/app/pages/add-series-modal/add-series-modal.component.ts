import { Component } from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {DynamicDialogRef} from 'primeng/dynamicdialog';
import {FormBuilder, FormGroup, ReactiveFormsModule, Validators} from '@angular/forms';
import {DropdownModule} from 'primeng/dropdown';
import {InputText} from 'primeng/inputtext';
import {StyleClass} from 'primeng/styleclass';

@Component({
  selector: 'app-add-series-modal',
  imports: [
    ReactiveFormsModule,
    DropdownModule,
    InputText,
    StyleClass
  ],
  templateUrl: './add-series-modal.component.html',
  styleUrl: './add-series-modal.component.scss'
})


export class AddSeriesModalComponent {
  seriesForm!: FormGroup;
  constructor(private readonly http:HttpClient, private readonly dialogRef:DynamicDialogRef, private readonly fb:FormBuilder) {
    this.seriesForm = this.fb.group({
      Title: ['', Validators.required],
      Genre_Id: [null, Validators.required],
      Age_Rating: ['', Validators.required]
    });
  }

  genres = [
    { label: 'Action', value: 1 },
    { label: 'Comedy', value: 2 },
    { label: 'Drama', value: 3 }
  ];

  ageRatings = [
    { label: 'G', value: 'G' },
    { label: 'PG', value: 'PG' },
    { label: 'PG-13', value: 'PG-13' },
    { label: 'R', value: 'R' }
  ];

  onSubmit() {
    if (this.seriesForm.valid) {
      this.http.post('http://localhost:5025/stored-procedure-create-series', this.seriesForm.value).subscribe(() => {
        this.dialogRef.close(true);
        }

      );
    }
  }

  onCancel() {
    this.dialogRef.close(false);
  }
}
