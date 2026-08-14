import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-score-badge',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div [ngClass]="getBadgeClass()" class="inline-flex items-center px-3 py-1 rounded-full text-xs font-bold shadow-sm">
      <span class="mr-1">AI Fit Score:</span>
      <span>{{ score ?? 0 }}%</span>
    </div>
  `
})
export class ScoreBadgeComponent {
  @Input() score: number | undefined = 0;

  getBadgeClass(): string {
    const val = this.score ?? 0;
    if (val >= 80) return 'bg-emerald-100 text-emerald-800 border border-emerald-300';
    if (val >= 60) return 'bg-amber-100 text-amber-800 border border-amber-300';
    return 'bg-rose-100 text-rose-800 border border-rose-300';
  }
}
