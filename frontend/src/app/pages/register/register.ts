import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
	selector: 'app-register',
	standalone: true,
	imports: [CommonModule, FormsModule, RouterLink],
	templateUrl: './register.html',
	styleUrl: './register.css'
})
export class Register {
	username = '';
	password = '';
	confirmPassword = '';
	errorMessage = '';
	successMessage = '';
	isLoading = false;

	constructor(
		private authService: AuthService,
		private router: Router
	) {}

	register(): void {
		const username = this.username.trim();
		const password = this.password;
		const confirmPassword = this.confirmPassword;

		this.errorMessage = '';
		this.successMessage = '';

		if (!username || !password || !confirmPassword) {
			this.errorMessage = 'Please complete all fields.';
			return;
		}

		if (password !== confirmPassword) {
			this.errorMessage = 'Passwords do not match.';
			return;
		}

		this.isLoading = true;

		this.authService.register(username, password).subscribe({
			next: () => {
				this.successMessage = 'Account created. Signing you in...';
				setTimeout(() => {
					this.authService.login(username, password).subscribe({
						next: () => {
							this.router.navigateByUrl('/products');
						},
						error: () => {
							this.isLoading = false;
							this.successMessage = '';
							this.errorMessage = 'Account created, but automatic sign-in failed. Please sign in from the login page.';
							setTimeout(() => this.router.navigateByUrl('/login'), 1200);
						}
					});
				}, 600);
			},
			error: (error: Error) => {
				this.errorMessage = error.message || 'Unable to create the account right now.';
			}
		}).add(() => {
			this.isLoading = false;
		});
	}
}
