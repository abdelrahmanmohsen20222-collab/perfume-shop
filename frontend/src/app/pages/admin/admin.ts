import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { Product, ProductCategory } from '../products/products.data';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-admin',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './admin.html',
  styleUrl: './admin.css'
})
export class Admin {
  email = '';
  password = '';
  isAuthorized = false;
  isLoading = false;
  message = '';
  name = '';
  category: ProductCategory = 'Floral';
  notes = '';
  price = 0;
  size = '50 ml';
  image = '';

  constructor(private authService: AuthService) {}

  unlock(): void {
    const email = this.email.trim();

    if (!email || !this.password) {
      this.message = 'Enter your admin email and password.';
      return;
    }

    this.isLoading = true;
    this.message = '';
    this.authService.login(email, this.password).subscribe({
      next: () => {
        if (!this.authService.isAdmin()) {
          this.authService.logout();
          this.isLoading = false;
          this.message = 'This account is not an administrator.';
          return;
        }

        this.isAuthorized = true;
        this.isLoading = false;
        this.password = '';
      },
      error: (error: Error) => {
        this.isLoading = false;
        this.message = error.message || 'Admin sign-in failed.';
      }
    });
  }

  addProduct(): void {
    if (!this.name.trim() || !this.notes.trim() || !this.price || !this.image.trim()) {
      this.message = 'Complete the product name, notes, price and image URL.';
      return;
    }

    const products = this.readCustomProducts();
    products.push({
      id: Date.now(),
      name: this.name.trim(),
      category: this.category,
      notes: this.notes.trim(),
      price: this.price,
      size: this.size,
      color: 'custom',
      image: this.image.trim(),
      badge: 'NEW'
    });
    localStorage.setItem('custom-products', JSON.stringify(products));
    this.message = 'Product added to the catalog.';
    this.name = '';
    this.notes = '';
    this.price = 0;
    this.image = '';
  }

  private readCustomProducts(): Product[] {
    try {
      return JSON.parse(localStorage.getItem('custom-products') ?? '[]') as Product[];
    } catch {
      return [];
    }
  }
}
