import { NgModule } from '@angular/core';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';

@NgModule({
    declarations: [],
    imports: [
        MatExpansionModule,
        MatButtonModule,
        MatFormFieldModule,
        MatInputModule
    ],
    exports: []
})
export class MaterialModule { }