import { Component } from '@angular/core';

@Component({
  selector: 'app-loading-view',
  standalone: false,
  templateUrl: './loading-view.html',
  styleUrls: ['./loading-view.scss']
})
export class LoadingView {
  isLoading: boolean = false;
}
