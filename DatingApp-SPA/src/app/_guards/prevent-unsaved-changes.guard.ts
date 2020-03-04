import { Injectable } from '@angular/core';
import { CanDeactivate } from '@angular/router';
import { MemberEditComponent } from '../member/member-edit/member-edit.component';
import { MemberEditResolver } from '../_resolvers/member-edit-resolver';
import { ComponentLoaderFactory } from 'ngx-bootstrap';

@Injectable()
export class PreventUnsaveChanges implements CanDeactivate<MemberEditComponent> {
    canDeactivate(component: MemberEditComponent) {
        if (component.editForm.dirty) {
            return confirm('Are you sure you want to continue? Any unsaved chnages will be lost.');
        }
        return true;
    }
}
