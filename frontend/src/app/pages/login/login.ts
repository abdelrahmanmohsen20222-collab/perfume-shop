import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
	selector: 'app-login',
	standalone: true,
	imports: [CommonModule, FormsModule, RouterLink],
	templateUrl: './login.html',
	styleUrl: './login.css'
})
export class Login {
	username = '';
	password = '';
	errorMessage = '';
	isLoading = false;

	constructor(
		private authService: AuthService,
		private router: Router
	) {}

	login(): void {
		const username = this.username.trim();
		const password = this.password;

		if (!username || !password) {
			this.errorMessage = 'Please enter your username and password.';
			return;
		}

		this.isLoading = true;
		this.errorMessage = '';

		this.authService.login(username, password).subscribe({
			next: () => {
				this.isLoading = false;
				this.router.navigateByUrl('/products');
			},
			error: (error: Error) => {
				this.isLoading = false;
				this.errorMessage = error.message || 'Invalid username or password.';
			}
		});
	}
}