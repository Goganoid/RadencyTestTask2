import { Injectable } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
// Bypass angular base64 protection
@Injectable({
  providedIn: 'root'
})
export class ImageSanitizerService {

  constructor(private sanitizer: DomSanitizer) { }
  public sanitizeImage(base64img:string) {
    return this.sanitizer.bypassSecurityTrustUrl(base64img);
  }
}
