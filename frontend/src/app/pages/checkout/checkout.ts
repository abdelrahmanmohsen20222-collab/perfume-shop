import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { CartService } from '../../services/cart.service';

@Component({
  selector: 'app-checkout',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './checkout.html',
  styleUrl: './checkout.css'
})
export class Checkout {
  fullName = '';
  phone = '';
  address = '';
  city = '';
  postalCode = '';
  paymentMethod = 'card';
  errorMessage = '';
  orderMessage = '';
  isSubmitted = false;

  constructor(public cartService: CartService) {}

  placeOrder(): void {
    this.errorMessage = '';

    if (!this.fullName.trim() || !this.phone.trim() || !this.address.trim() || !this.city.trim() || !this.postalCode.trim()) {
      this.errorMessage = 'Please complete your delivery information.';
      return;
    }

    if (!this.cartService.totalQuantity) {
      this.errorMessage = 'Your bag is empty. Add a fragrance before checkout.';
      return;
    }

    this.isSubmitted = true;
    this.orderMessage = `Thank you, ${this.fullName.trim()}. Your order is confirmed.`;
    this.cartService.clear();
  }
}
