import { TestBed } from '@angular/core/testing';

import { ImageSanitizerService } from './image-sanitizer.service';

describe('ImageSanitizerService', () => {
  let service: ImageSanitizerService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ImageSanitizerService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
