import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-image-viewer',
  templateUrl: './image-viewer.component.html',
  styleUrls: ['./image-viewer.component.css']
})
export class ImageViewerComponent implements OnInit{

  images!: Array<string>
  prevActive = false;
  nextActive = false;
  activeImage = 0;

  constructor(@Inject(MAT_DIALOG_DATA) public data: {images: Array<string>}, public dialogRef: MatDialogRef<ImageViewerComponent>, ){}

  ngOnInit(): void {
    this.images = this.data.images
    if(this.images.length > 1){
      this.nextActive = true;
    }
  }

  previous(){
    this.activeImage--
    this.nextActive = true
    if(this.activeImage === 0){
      this.prevActive = false
    }
  }

  next(){
    this.activeImage++
    this.prevActive = true
    if(this.images.length === this.activeImage + 1){
      this.nextActive = false
    }
  }

}
