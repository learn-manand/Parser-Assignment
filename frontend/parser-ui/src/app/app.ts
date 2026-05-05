import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { ChangeDetectorRef } from '@angular/core';

import { ParserService } from '../services/parser.service';
import { ParseResponse } from '../models/parse-response';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, FormsModule, HttpClientModule],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {

  private parserService = inject(ParserService);
  private cdr = inject(ChangeDetectorRef);

  inputText = '';
  result: ParseResponse | null = null;
  resultJson = '';
  error = '';
  loading = false;

  submit(form: any): void {
    if (form.invalid) {
      return;
    }

    this.error = '';
    this.resultJson = '';
    this.loading = true;

    this.parserService.parseText(this.inputText).subscribe({
      next: res => {
        this.resultJson = JSON.stringify(res, null, 2);
        this.loading = false;
        this.cdr.detectChanges();
      },
      error: err => {
        this.error = err.error?.error || 'Something went wrong.';
        this.loading = false;
        this.cdr.detectChanges();
      }
    });
  }

  clear(form: any): void {
    form.resetForm();
    this.inputText = '';
    this.resultJson = '';
    this.error = '';
  }
}