import { Routes } from '@angular/router';
import { Login } from './pages/login/login';
import { Products } from './pages/products/products';
import { Register } from './pages/register/register';
import { Cart } from './pages/cart/cart';
import { Checkout } from './pages/checkout/checkout';
import { Admin } from './pages/admin/admin';
import { ScentFinder } from './pages/scent-finder/scent-finder';

export const routes: Routes = [
  {
    path: 'login',
    component: Login
  },
  {
    path: 'register',
    component: Register
  },
  {
    path: 'products',
    component: Products
  },
  {
    path: 'cart',
    component: Cart
  },
  {
    path: 'checkout',
    component: Checkout
  },
  {
    path: 'admin',
    component: Admin
  },
  {
    path: 'scent-finder',
    component: ScentFinder
  },
  {
    path: '',
    redirectTo: 'products',
    pathMatch: 'full'
  }
];
