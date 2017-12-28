import { ModerationScore } from './moderation-score';

export interface ModerationResponse {
    pass: boolean;
    moderationScores: ModerationScore[];
}