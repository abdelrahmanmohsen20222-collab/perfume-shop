import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { CartItem, CartService } from '../../services/cart.service';

@Component({
  selector: 'app-cart',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './cart.html',
  styleUrl: './cart.css'
})
export class Cart {
  constructor(
    public cartService: CartService,
    private router: Router
  ) {}

  get items(): CartItem[] {
    return this.cartService.cartItems;
  }

  updateQuantity(item: CartItem, quantity: number): void {
    this.cartService.updateQuantity(item.product.id, quantity);
  }

  remove(productId: number): void {
    this.cartService.remove(productId);
  }

  checkout(): void {
    this.router.navigateByUrl('/checkout');
  }
}
