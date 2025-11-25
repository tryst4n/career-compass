import { HttpClient, HttpClientModule } from '@angular/common/http';
import { Component } from '@angular/core';
import { Router, RouterModule, RouterOutlet } from '@angular/router';
const domain = "http://localhost:5063/";

@Component({
  selector: 'app-home',
  imports: [RouterOutlet, RouterModule, HttpClientModule],
  templateUrl: './home.html',
  styleUrl: './home.css'
})
export class Home {
  constructor(public router: Router, public http: HttpClient) { }
  isLogged() {
    if (sessionStorage.getItem("token") != null) {
      return true;
    }
    return false
  }

  getUsername() {
    return sessionStorage.getItem("username");
  }

  async logout() {
    sessionStorage.clear();
    this.router.navigate(["/login"]);
  }

  //states
  analysis: string = '';
  loadingAnalysis: boolean = false;

  async analyzeResume() {
    //get input values
    const textarea = document.getElementById('resume') as HTMLTextAreaElement;
    const input = document.getElementById('jobInterests') as HTMLInputElement;

    const resume = textarea?.value.trim() ?? '';
    const jobInterests = input?.value.trim() ?? '';

    const analysisEl = document.getElementById('analysis');
    if (analysisEl) analysisEl.textContent = '';

    //check for resume
    if (!resume) {
      if (analysisEl) analysisEl.textContent = 'Please provide your resume!';
      return;
    }

    this.loadingAnalysis = true;
    if (analysisEl) analysisEl.textContent = 'Analyzing...';

    try {
      const payload = { ResumeText: resume, JobInterests: jobInterests };

      const response = await this.http.post(domain + 'api/AI/analyze', payload, {
        responseType: 'text'
      }).toPromise();

      const openaiResponse = JSON.parse(response as string);
      this.analysis = openaiResponse.reply;

      if (analysisEl) analysisEl.textContent = openaiResponse.reply;

    } catch (err) {
      const msg = err instanceof Error ? err.message : String(err);
      if (analysisEl) analysisEl.textContent = `Error: ${msg}`;
      console.error(err);
    } finally {
      this.loadingAnalysis = false;
    }
  }
}
