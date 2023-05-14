import { HttpErrorResponse } from "@angular/common/http";
import { FormGroup } from "@angular/forms";

export function validationHandler(error: Error, form: FormGroup) {
  if (error instanceof HttpErrorResponse && error.status === 400) {

    if (error.error.type === 'hotelError') {
      const backendErrors = error.error.errors;

      for (const backendError of backendErrors) {
        const formControl = form.get(backendError.fieldName.charAt(0).toLowerCase() + backendError.fieldName.slice(1));
        if (formControl) {
            formControl.setErrors({ serverError: backendError.fieldErrorMessage });
        }
      }
    }
    else {
      const backendErrors = error.error.errors;

      for (const key of Object.keys(backendErrors)) {
        const formControl = form.get(key.charAt(0).toLowerCase() + key.slice(1));

        if (formControl) {
          for (const message of backendErrors[key]) {
            formControl.setErrors({ serverError: message });
          }
        }
      }
    }

  }
}
