import { Component, OnInit, inject } from '@angular/core';
import { GlobalConstants } from '../shared/global-constants';
import { Rule } from './rule.model';
import { RulesService } from './rules.service';
import { AccordionComponent } from '../shared/accordion/accordion.component';

@Component({
    selector: 'app-rules',
    templateUrl: './rules.component.html',
    styleUrls: ['./rules.component.css'],
    imports: [AccordionComponent]
})
export class RulesComponent implements OnInit {
  private readonly rulesService = inject(RulesService);

  rules: Rule[] = [];
  year = GlobalConstants.year;
  edition = GlobalConstants.editionNumber;

  ngOnInit(): void {
    this.rules = this.rulesService.get();
  }
}
