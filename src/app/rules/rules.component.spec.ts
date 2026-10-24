import { TestBed } from '@angular/core/testing';
import { provideNoopAnimations } from '@angular/platform-browser/animations';
import { describe, expect, it } from 'vitest';

import { GlobalConstants } from '../shared/global-constants';
import { RulesComponent } from './rules.component';
import { RulesService } from './rules.service';

describe('RulesComponent', () => {
  it('pinta un panel por cada regla con su título', () => {
    TestBed.configureTestingModule({ imports: [RulesComponent], providers: [provideNoopAnimations()] });
    const fixture = TestBed.createComponent(RulesComponent);
    fixture.detectChanges();

    const el = fixture.nativeElement as HTMLElement;
    const rules = TestBed.inject(RulesService).get();
    const panels = el.querySelectorAll('mat-expansion-panel');
    expect(panels.length).toBe(rules.length);
    expect(panels[0].textContent).toContain(rules[0].title);
    expect(el.querySelector('.page-subtitle')?.textContent).toContain(GlobalConstants.year);
  });
});
