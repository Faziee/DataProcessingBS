import { Component, OnInit } from '@angular/core';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import {ButtonDirective} from 'primeng/button';
import {NgIf} from '@angular/common';

@Component({
  selector: 'app-gif-display',
  imports: [
    ButtonDirective,
    NgIf
  ],
  template: `
    <div class="p-6 text-center">
      <h2 class="text-xl font-semibold mb-4 text-red-500">
        {{ config.data.title }}
      </h2>

      <div class="flex justify-center my-4">
        <img
          [src]="config.data.gifUrl"
          [alt]="config.data.title"
          class="max-w-full h-auto rounded-lg shadow-lg"
        >
      </div>

      <p *ngIf="config.data.description" class="text-red-500 mt-4">
        {{ config.data.description }}
      </p>

      <div class="flex justify-end mt-6">
        <button
          pButton
          (click)="close()"
          label="Close"
          class="p-button-secondary"
        >
        </button>
      </div>
    </div>
  `
})
export class GifDisplayComponent implements OnInit {
  constructor(
    public config: DynamicDialogConfig,
    private ref: DynamicDialogRef
  ) {}

  ngOnInit() {
    if (!this.config.data?.gifUrl) {
      console.error('GIF URL is required');
      this.close();
    }
  }

  close() {
    this.ref.close();
  }
}
