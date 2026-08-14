import { Component } from '@angular/core';

@Component({
  selector: 'app-footer',
  standalone: true,
  template: `
    <footer class="bg-slate-900 text-gray-400 py-8 mt-12 border-t border-slate-800">
      <div class="max-w-7xl mx-auto px-4 text-center">
        <p class="text-sm">&copy; 2026 Smart Recruitment Platform. Cognitive .NET 10 + Angular 22 Solution.</p>
      </div>
    </footer>
  `
})
export class FooterComponent {}
