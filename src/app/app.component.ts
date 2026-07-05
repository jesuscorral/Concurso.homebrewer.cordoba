import { Component, ElementRef, OnInit, Renderer2, ViewChild, inject } from '@angular/core';
import { NavigationEnd, Router, RouterOutlet } from '@angular/router';
import { Subscription } from 'rxjs';
import { filter } from 'rxjs/operators';

import { NavbarComponent } from './shared/navbar/navbar.component';
import { FooterComponent } from './shared/footer/footer.component';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.scss'],
    imports: [NavbarComponent, RouterOutlet, FooterComponent]
})
export class AppComponent implements OnInit {
    private readonly renderer = inject(Renderer2);
    private readonly router = inject(Router);
    private readonly element = inject(ElementRef);

    private routerSubscription: Subscription;
    @ViewChild(NavbarComponent) navbar: NavbarComponent;

    ngOnInit() {
        const navbar: HTMLElement = this.element.nativeElement.children[0].children[0];
        this.routerSubscription = this.router.events
            .pipe(filter((event) => event instanceof NavigationEnd))
            .subscribe(() => {
                if (window.outerWidth > 991) {
                    window.document.children[0].scrollTop = 0;
                } else if (window.document.activeElement) {
                    window.document.activeElement.scrollTop = 0;
                }
            });
        this.renderer.listen('window', 'scroll', () => {
            const scrollPosition = window.scrollY || window.pageYOffset;
            if (scrollPosition > 150) {
                navbar.classList.remove('navbar-transparent');
            } else {
                navbar.classList.add('navbar-transparent');
            }
        });
    }
}
