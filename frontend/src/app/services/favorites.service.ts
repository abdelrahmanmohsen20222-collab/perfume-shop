import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class FavoritesService {
  private readonly storageKey = 'perfume-favorites';
  private productIds = this.readIds();

  get count(): number {
    return this.productIds.length;
  }

  has(productId: number): boolean {
    return this.productIds.includes(productId);
  }

  toggle(productId: number): void {
    this.productIds = this.has(productId)
      ? this.productIds.filter(id => id !== productId)
      : [...this.productIds, productId];
    localStorage.setItem(this.storageKey, JSON.stringify(this.productIds));
  }

  private readIds(): number[] {
    try {
      const saved = JSON.parse(localStorage.getItem(this.storageKey) ?? '[]');
      return Array.isArray(saved) ? saved.filter(Number.isInteger) : [];
    } catch {
      return [];
    }
  }
}
