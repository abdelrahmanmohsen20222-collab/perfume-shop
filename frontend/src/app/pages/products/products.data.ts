export type ProductCategory = 'Floral' | 'Woody' | 'Amber' | 'Fresh';

export interface Product {
  id: number;
  name: string;
  category: ProductCategory;
  notes: string;
  price: number;
  size: string;
  color: string;
  image: string;
  badge?: string;
}

export const PRODUCTS: Product[] = [
  { id: 1, name: 'Rose Elan', category: 'Floral', notes: 'Rose, iris and soft musk', price: 96, size: '50 ml', color: 'rose', image: 'https://images.unsplash.com/photo-1588405748880-12d1d2a59f75?auto=format&fit=crop&w=900&q=85', badge: 'BESTSELLER' },
  { id: 2, name: 'Amber No. 01', category: 'Amber', notes: 'Vanilla, amber and cedarwood', price: 89, size: '50 ml', color: 'amber', image: 'https://images.unsplash.com/photo-1594035910387-fea47794261f?auto=format&fit=crop&w=900&q=85', badge: 'NEW' },
  { id: 3, name: 'Cedar Atelier', category: 'Woody', notes: 'Bergamot, cedar and vetiver', price: 92, size: '50 ml', color: 'cedar', image: 'https://images.unsplash.com/photo-1594035910387-fea47794261f?auto=format&fit=crop&w=900&q=85' },
  { id: 4, name: 'Lumiere Verte', category: 'Fresh', notes: 'Citrus, green tea and neroli', price: 84, size: '50 ml', color: 'green', image: 'https://images.unsplash.com/photo-1547887538-e3a2f32cb1cc?auto=format&fit=crop&w=900&q=85' },
  { id: 5, name: 'Velvet Oud', category: 'Woody', notes: 'Oud, saffron and sandalwood', price: 118, size: '75 ml', color: 'violet', image: 'https://images.unsplash.com/photo-1592945403244-b3fbafd7f539?auto=format&fit=crop&w=900&q=85' },
  { id: 6, name: 'Peony Mist', category: 'Floral', notes: 'Peony, pear and white musk', price: 78, size: '50 ml', color: 'peach', image: 'https://images.unsplash.com/photo-1563170351-be82bc888aa4?auto=format&fit=crop&w=900&q=85' }
];
