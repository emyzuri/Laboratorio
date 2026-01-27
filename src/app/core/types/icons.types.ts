import { IconPack } from '@fortawesome/angular-fontawesome';
import { IconProp } from '@fortawesome/fontawesome-svg-core';
import { faMicrosoft, faFacebook } from '@fortawesome/free-brands-svg-icons';
import {
  faCancel,
  faCircleExclamation,
  faDatabase,
  faEye,
  faFileLines,
  faGear,
  faHouse,
  faMagnifyingGlass,
  faPlus,
  faRightLeft,
  faSquare,
  faBoxesPacking,
  faTag,
  faBuilding,
  faUserGroup,
  faBars,
  faChevronDown,
  faPencil,
  faTrash,
  faTimes,
  faSpinner,
} from '@fortawesome/free-solid-svg-icons';

const iconPack: IconPack = {
  faMicrosoft,
  faFacebook,
  faCancel,
  faCircleExclamation,
  faHouse,
  faGear,
  faUserGroup,
  faBuilding,
  faFileLines,
  faRightLeft,
  faTag,
  faDatabase,
  faBoxesPacking,
  faSquare,
  faPlus,
  faMagnifyingGlass,
  faEye,
  faBars,
  faChevronDown,
  faPencil,
  faTrash,
  faTimes,
  faSpinner,
};

declare type IconName =
  | 'microsoft'
  | 'facebook'
  | 'cancel'
  | 'circle-exclamation'
  | 'house'
  | 'gear'
  | 'user-group'
  | 'building'
  | 'file-lines'
  | 'right-left'
  | 'tag'
  | 'database'
  | 'boxes-packing'
  | 'square'
  | 'plus'
  | 'search'
  | 'eye'
  | 'bars'
  | 'chevron-down'
  | 'edit'
  | 'delete'
  | 'close'
  | 'spinner';

const iconMap: Record<IconName, IconProp> = {
  microsoft: faMicrosoft,
  facebook: faFacebook,
  cancel: faCancel,
  'circle-exclamation': faCircleExclamation,
  house: faHouse,
  gear: faGear,
  'user-group': faUserGroup,
  building: faBuilding,
  'file-lines': faFileLines,
  'right-left': faRightLeft,
  tag: faTag,
  database: faDatabase,
  'boxes-packing': faBoxesPacking,
  square: faSquare,
  plus: faPlus,
  search: faMagnifyingGlass,
  eye: faEye,
  bars: faBars,
  'chevron-down': faChevronDown,
  edit: faPencil,
  delete: faTrash,
  close: faTimes,
  spinner: faSpinner,
};

export { iconPack, type IconName, iconMap };
