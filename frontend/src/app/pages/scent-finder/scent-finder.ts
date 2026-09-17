import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { ApiService } from '../../services/api.service';

@Component({
  selector: 'app-scent-finder',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './scent-finder.html',
  styleUrl: './scent-finder.css'
})
export class ScentFinder {
  mood = 'Warm and comforting';
  notes = '';
  occasion = 'Everyday';
  strength = 'Soft and close to the skin';
  recommendation = '';
  errorMessage = '';
  isLoading = false;

  constructor(private api: ApiService) {}

  findMyScent(): void {
    if (!this.notes.trim()) {
      this.errorMessage = 'Tell us at least one note or scent you enjoy.';
      return;
    }

    this.isLoading = true;
    this.errorMessage = '';
    this.recommendation = '';

    this.api.findScent({
      mood: this.mood,
      notes: this.notes.trim(),
      occasion: this.occasion,
      strength: this.strength
    }).subscribe({
      next: ({ recommendation }) => {
        this.recommendation = recommendation;
        this.isLoading = false;
      },
      error: (error: Error) => {
        this.isLoading = false;
        this.errorMessage = error.message || 'We could not find a scent match right now. Please try again.';
      }
    });
  }
}
