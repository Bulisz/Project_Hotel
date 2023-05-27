import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'split'
})
export class SplitPipe implements PipeTransform {

  transform(text: string, by: string) {
    let arr = text.split(by);
    return arr[arr.length - 1]
  }

}
