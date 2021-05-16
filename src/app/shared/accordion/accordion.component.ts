import { Component, Input, OnInit } from '@angular/core';
import { Rule } from 'app/rules/rule.model';

@Component({
  selector: 'app-accordion',
  templateUrl: './accordion.component.html',
  styleUrls: ['./accordion.component.scss']
})
export class AccordionComponent implements OnInit {

  @Input() rows: Rule[];

  constructor() { }

  ngOnInit(): void {
  }

}
