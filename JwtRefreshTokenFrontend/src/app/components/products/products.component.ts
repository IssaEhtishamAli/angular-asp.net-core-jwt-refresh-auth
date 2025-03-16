import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

interface Product {
  id: number;
  name: string;
  price: number;
}

@Component({
  selector: 'app-product',
  template: `
    <h2>Product List</h2>
    <div *ngIf="!products">
      Loading products...
    </div>
    <ul>
      <li *ngFor="let product of products">
        {{product.name}} - {{product.price}}
      </li>
    </ul>
  `
})
export class ProductComponent implements OnInit {
  products: Product[] | null = null;
  private baseUrl = 'https://localhost:7144/api/product';

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.http.get<Product[]>(this.baseUrl).subscribe({
      next: data => {
        this.products = data;
      },
      error: err => {
        console.error('Failed to load products', err);
      }
    });
  }
}
