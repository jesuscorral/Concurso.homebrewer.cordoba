import { Component, OnInit } from '@angular/core';
import { Rule } from './rule.model';
import { RulesService } from './rules.service';

@Component({
  selector: 'app-rules',
  templateUrl: './rules.component.html',
  styleUrls: ['./rules.component.css']
})
export class RulesComponent implements OnInit {
  rules: Rule[] = [];

  constructor(private rulesService: RulesService)
  {}

  ngOnInit(): void {
    this.rules = this.rulesService.get();
  }

}
