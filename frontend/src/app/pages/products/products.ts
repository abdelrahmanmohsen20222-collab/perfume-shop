import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { CartService } from '../../services/cart.service';
import { FavoritesService } from '../../services/favorites.service';
import { Product, ProductCategory, PRODUCTS } from './products.data';

@Component({
	selector: 'app-products',
	standalone: true,
	imports: [CommonModule, FormsModule, RouterLink],
	templateUrl: './products.html',
	styleUrl: './products.css'
})
export class Products {
	readonly categories: Array<'All' | ProductCategory> = ['All', 'Floral', 'Woody', 'Amber', 'Fresh'];
	searchTerm = '';
	selectedCategory: 'All' | ProductCategory = 'All';
	sortBy: 'featured' | 'price-low' | 'price-high' | 'name' = 'featured';
	showFavoritesOnly = false;
	addedProduct = '';

	constructor(
		private cartService: CartService,
		private favoritesService: FavoritesService
	) {}

	get cartCount(): number {
		return this.cartService.totalQuantity;
	}

	get favoritesCount(): number {
		return this.favoritesService.count;
	}

	get filteredProducts(): Product[] {
		const products = this.readProducts();
		const search = this.searchTerm.trim().toLowerCase();

		const filtered = products.filter((product) => {
			const matchesCategory = this.selectedCategory === 'All' || product.category === this.selectedCategory;
			const matchesSearch = !search || `${product.name} ${product.notes} ${product.category}`.toLowerCase().includes(search);
			const matchesFavorites = !this.showFavoritesOnly || this.favoritesService.has(product.id);
			return matchesCategory && matchesSearch && matchesFavorites;
		});

		return [...filtered].sort((first, second) => {
			switch (this.sortBy) {
				case 'price-low': return first.price - second.price;
				case 'price-high': return second.price - first.price;
				case 'name': return first.name.localeCompare(second.name);
				default: return 0;
			}
		});
	}

	private readProducts(): Product[] {
		try {
			const customProducts = JSON.parse(localStorage.getItem('custom-products') ?? '[]') as Product[];
			return [...PRODUCTS, ...customProducts];
		} catch {
			return PRODUCTS;
		}
	}

	addToCart(product: Product): void {
		this.cartService.add(product);
		this.addedProduct = product.name;
		setTimeout(() => {
			if (this.addedProduct === product.name) {
				this.addedProduct = '';
			}
		}, 1800);
	}

	isFavorite(product: Product): boolean {
		return this.favoritesService.has(product.id);
	}

	toggleFavorite(product: Product): void {
		this.favoritesService.toggle(product.id);
	}

}
