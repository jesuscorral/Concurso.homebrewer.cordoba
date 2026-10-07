import { Component, OnInit, inject } from '@angular/core';
import { RouterOutlet } from '@angular/router';

import { NavbarComponent } from './shared/navbar/navbar.component';
import { FooterComponent } from './shared/footer/footer.component';
import { CookieBannerComponent } from './shared/cookie-consent/cookie-banner.component';
import { SeoService } from './shared/seo/seo.service';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.scss'],
    imports: [NavbarComponent, RouterOutlet, FooterComponent, CookieBannerComponent]
})
export class AppComponent implements OnInit {
    private readonly seo = inject(SeoService);

    ngOnInit(): void {
        this.seo.init();
    }
}
