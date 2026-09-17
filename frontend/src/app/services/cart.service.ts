import { Injectable } from '@angular/core';
import { Product } from '../pages/products/products.data';

export interface CartItem {
  product: Product;
  quantity: number;
}

@Injectable({ providedIn: 'root' })
export class CartService {
  private readonly storageKey = 'perfume-cart';
  private items: CartItem[] = this.readItems();

  get cartItems(): CartItem[] {
    return this.items;
  }

  get totalQuantity(): number {
    return this.items.reduce((total, item) => total + item.quantity, 0);
  }

  get subtotal(): number {
    return this.items.reduce((total, item) => total + item.product.price * item.quantity, 0);
  }

  add(product: Product): void {
    const item = this.items.find((entry) => entry.product.id === product.id);

    if (item) {
      item.quantity += 1;
    } else {
      this.items.push({ product, quantity: 1 });
    }

    this.saveItems();
  }

  updateQuantity(productId: number, quantity: number): void {
    const item = this.items.find((entry) => entry.product.id === productId);

    if (!item) {
      return;
    }

    if (quantity <= 0) {
      this.remove(productId);
      return;
    }

    item.quantity = quantity;
    this.saveItems();
  }

  remove(productId: number): void {
    this.items = this.items.filter((item) => item.product.id !== productId);
    this.saveItems();
  }

  clear(): void {
    this.items = [];
    this.saveItems();
  }

  private readItems(): CartItem[] {
    try {
      const storedItems = localStorage.getItem(this.storageKey);
      return storedItems ? JSON.parse(storedItems) as CartItem[] : [];
    } catch {
      return [];
    }
  }

  private saveItems(): void {
    localStorage.setItem(this.storageKey, JSON.stringify(this.items));
  }
}
